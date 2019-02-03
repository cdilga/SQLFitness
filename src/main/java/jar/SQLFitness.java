package jar;

import java.util.ArrayList;

public class SQLFitness {

    public static void main(String[] args) {
        StringBuilder question = new StringBuilder();
        for (int i = 0;i<args.length;i++)
        {
            question.append(args[i] + ' ');
        }
        ;
        QuestionParser parser = new QuestionParser(question.toString());
        ArrayList keywords = parser.keywords();
        System.out.println(keywords);
    }
}