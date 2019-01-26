package jar;

import java.util.ArrayList;

public class SQLFitness {

    public static void main(String[] args) {
        QuestionParser question = new QuestionParser(args.toString());
        ArrayList keywords = question.keywords();
        System.out.println(keywords);
    }
}